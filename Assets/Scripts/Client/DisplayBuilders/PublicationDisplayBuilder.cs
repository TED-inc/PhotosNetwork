using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public delegate void OpenComments(int commentId);
    public sealed class PublicationDisplayBuilder
    {
        private const int loadCount = 5;
        private const int maxPublicationsCount = 10;

        private IServerConnection connection;
        private IClientCommentService commentService;
        private RectTransform publicationsParent;
        private PublicationDisplay publicationPrefab;
        private Dictionary<int, PublicationDisplay> publicationInstances 
            = new Dictionary<int, PublicationDisplay>(maxPublicationsCount);

        private int lastPublicationId;
        private int firstPublicationId = int.MaxValue;

        private string DirectoryCashePath => $"{Application.temporaryCachePath}/Images";
        private string GetFilePathInCahse(int id) => $"{DirectoryCashePath}/{id}.jpg";

        public void Load(GetDataMode dataMode = GetDataMode.After)
        {
            bool firstRun = publicationInstances.Count == 0;

            if (firstRun)
                connection.PublicationService.GetPublications(loadCount, Callback);
            else
                connection.PublicationService.GetPublications(
                    dataMode == GetDataMode.After ?
                        lastPublicationId :
                        firstPublicationId,
                    loadCount, dataMode, Callback);



            void Callback((Publication publication, User user)[] publications, Result result)
            {
                if (result != Result.Failed)
                {
                    if (!firstRun)
                        CoroutineRunner.Instance.StartCoroutine(PreventCurrentPublicatioScrollout());

                    RemoveFarawayPublications();

                    foreach ((Publication publication, User user) publicationData in publications)
                    {
                        if (publicationInstances.ContainsKey(publicationData.publication.Id))
                            continue;

                        ActualizeLastAndFirstPublicationsIds(publicationData.publication.Id);
                        PublicationDisplay instance = CreatePublicationInsatnce(
                            publicationData.publication.Id, 
                            publicationData.user.Username);
                        SetPhoto(instance, publicationData.publication.PhotoId);
                    }
                }



                void RemoveFarawayPublications()
                {
                    int removeCount = publicationInstances.Count + publications.Length - maxPublicationsCount;

                    if (removeCount > 0)
                    {
                        int[] idToDestoy = new int[removeCount];
                        IOrderedEnumerable<KeyValuePair<int, PublicationDisplay>> publicationsOrdered =
                            dataMode == GetDataMode.Before ?
                                publicationInstances.OrderByDescending(pair => pair.Key) :
                                publicationInstances.OrderBy(pair => pair.Key);

                        for (int i = 0; i < removeCount; i++)
                        {
                            KeyValuePair<int, PublicationDisplay> toDestoy = publicationsOrdered.ElementAt(i);
                            idToDestoy[i] = toDestoy.Key;
                            GameObject.Destroy(toDestoy.Value.gameObject);
                        }

                        if (dataMode == GetDataMode.Before)
                            lastPublicationId = publicationsOrdered.ElementAt(removeCount).Key;
                        else
                            firstPublicationId = publicationsOrdered.ElementAt(removeCount).Key;

                        foreach (int id in idToDestoy)
                            publicationInstances.Remove(id);
                    }
                }

                void ActualizeLastAndFirstPublicationsIds(int publicationId)
                {
                    if (publicationId > lastPublicationId)
                        lastPublicationId = publicationId;
                    if (publicationId < firstPublicationId)
                        firstPublicationId = publicationId;
                }

                PublicationDisplay CreatePublicationInsatnce(int publicationId, string username)
                {
                    PublicationDisplay instance = GameObject.Instantiate(publicationPrefab, publicationsParent);
                    publicationInstances.Add(publicationId, instance);

                    if (dataMode == GetDataMode.After)
                        instance.transform.SetAsFirstSibling();
                    instance.Setup(username, publicationId, commentService.ShowPage);

                    return instance;
                }

                void SetPhoto(PublicationDisplay instance, int photoId)
                {
                    string cashePath = GetFilePathInCahse(photoId);

                    if (File.Exists(cashePath))
                    {
                        Texture2D texture = new Texture2D(0, 0);
                        texture.LoadImage(File.ReadAllBytes(cashePath));
                        instance.SetTexture(texture);
                    }
                    else
                        connection.PublicationService.GetPhoto(photoId, PhotoLoadCallback);



                    void PhotoLoadCallback(PhotoData photoData, Result photoCallbackResult)
                    {
                        if (photoCallbackResult != Result.Failed)
                        {
                            File.WriteAllBytes(GetFilePathInCahse(photoData.Id), photoData.Data);
                            Texture2D texture = new Texture2D(0, 0);
                            texture.LoadImage(photoData.Data);
                            instance.SetTexture(texture);
                        }
                    }
                }

                IEnumerator PreventCurrentPublicatioScrollout()
                {
                    int anchorPublicationId = dataMode == GetDataMode.Before ? firstPublicationId : lastPublicationId;
                    float anchorPositon = publicationInstances[anchorPublicationId].transform.position.y;

                    yield return null;

                    float newAnchorPos = publicationInstances[anchorPublicationId].transform.position.y;
                    if (Mathf.Abs(anchorPositon - newAnchorPos) > 0.1f)
                        publicationsParent.Translate(Vector3.up * (anchorPositon - newAnchorPos), Space.World);
                }
            }
        }

        public PublicationDisplayBuilder(IServerConnection connection, IClientCommentService commentService, ClientPublicationServiceSerialization serviceSerialization)
        {
            this.connection = connection;
            this.commentService = commentService;
            publicationsParent = serviceSerialization.publicationsParent;
            publicationPrefab = serviceSerialization.publicationPrefab;
            if (!Directory.Exists(DirectoryCashePath))
                Directory.CreateDirectory(DirectoryCashePath);
        }
    }
}