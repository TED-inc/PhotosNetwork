using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public class PublicationInstanceFactory
    {
        private const int loadCount = 5;
        private const int maxPublicationsCount = 50;

        private IServerConnection connection;
        private Transform publicationsParent;
        private PublicationInstance publicationPrefab;
        private Dictionary<int, PublicationInstance> publicationInstances = new Dictionary<int, PublicationInstance>(maxPublicationsCount);

        private string DirectoryCashePath => $"{Application.temporaryCachePath}/Images";
        private string GetFilePathInCahse(int id) => $"{DirectoryCashePath}/{id}.jpg";

        public void Load(GetDataMode dataMode = GetDataMode.After)
        {
            if (publicationInstances.Count == 0)
                connection.PublicationService.GetPublications(loadCount, Callback);



            void Callback((Publication publication, User user)[] publications, Result result)
            {
                if (result != Result.Failed)
                    foreach ((Publication publication, User user) publicationData in publications)
                    {
                        if (publicationInstances.ContainsKey(publicationData.publication.Id))
                            continue;

                        string cashePath = GetFilePathInCahse(publicationData.publication.PhotoId);
                        PublicationInstance instance = GameObject.Instantiate(publicationPrefab, publicationsParent);
                        publicationInstances.Add(publicationData.publication.Id, instance);

                        if (dataMode == GetDataMode.Before)
                            instance.transform.SetAsFirstSibling();
                        instance.Setup(publicationData.user.Username, publicationData.publication.Id);

                        if (File.Exists(cashePath))
                        {
                            Texture2D texture = new Texture2D(0, 0);
                            texture.LoadImage(File.ReadAllBytes(cashePath));
                            instance.SetTexture(texture);
                        }
                        else
                            connection.PublicationService.GetPhoto(publicationData.publication.PhotoId,
                                (photoData, photoCallbackResult) => PhotoLoadCallback(photoData, photoCallbackResult, instance));
                    }


                void PhotoLoadCallback(PhotoData photoData, Result photoCallbackResult, PublicationInstance instance)
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

        public PublicationInstanceFactory(IServerConnection connection, Transform publicationsParent, PublicationInstance publicationPrefab)
        {
            this.connection = connection;
            this.publicationsParent = publicationsParent;
            this.publicationPrefab = publicationPrefab;
            if (!Directory.Exists(DirectoryCashePath))
                Directory.CreateDirectory(DirectoryCashePath);
        }
    }
}