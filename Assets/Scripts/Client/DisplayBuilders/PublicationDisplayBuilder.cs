using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public delegate void OpenComments(int commentId);
    public sealed class PublicationDisplayBuilder
    {
        private const int loadCount = 5;
        private const int maxPublicationsCount = 50;

        private IServerConnection connection;
        private IClientCommentService commentService;
        private Transform publicationsParent;
        private PublicationDisplay publicationPrefab;
        private Dictionary<int, PublicationDisplay> publicationInstances = new Dictionary<int, PublicationDisplay>(maxPublicationsCount);
        private int lastPublicationId;
        private long lastPublicationTime;
        private int firstPublicationId;
        private long firstPublicationTime = long.MaxValue;

        private string DirectoryCashePath => $"{Application.temporaryCachePath}/Images";
        private string GetFilePathInCahse(int id) => $"{DirectoryCashePath}/{id}.jpg";

        public void Load(GetDataMode dataMode = GetDataMode.After)
        {
            if (publicationInstances.Count == 0)
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
                    foreach ((Publication publication, User user) publicationData in publications)
                    {
                        if (publicationInstances.ContainsKey(publicationData.publication.Id))
                            continue;

                        ActualizeLastAndFirstPublicationsIds(publicationData.publication);
                        PublicationDisplay instance = CreatePublicationInsatnce(
                            publicationData.publication.Id, 
                            publicationData.user.Username);
                        SetPhoto(instance, publicationData.publication.PhotoId);
                    }
                }



                void ActualizeLastAndFirstPublicationsIds(Publication publication)
                {
                    if (publication.DataTimeUTC > lastPublicationTime)
                    {
                        lastPublicationTime = publication.DataTimeUTC;
                        lastPublicationId = publication.Id;
                    }
                    if (publication.DataTimeUTC < firstPublicationTime)
                    {
                        firstPublicationTime = publication.DataTimeUTC;
                        firstPublicationId = publication.Id;
                    }
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