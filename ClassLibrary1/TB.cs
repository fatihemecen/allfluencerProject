

using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Util;


namespace Data
{
    public class TB
    {
        //string storageAccountName = "";
        string storageAccountKey = "";
        string storageAccountKeyReadOnly = "";
        //public CloudStorageAccount db;
        public TB()
        {
            //storageAccountName = "argefiles";
            //storageAccountKey = "pVNINK9cd4sWodAtJJ46Z3hMPVGCHTPGrv+eGoM9IXWqsf4s/lMCnNl1R4QHpGeE8SjoFKM/n7ZdIlrNME8Q3A==";
            storageAccountKey = "DefaultEndpointsProtocol=https;AccountName=argefiles;AccountKey=pVNINK9cd4sWodAtJJ46Z3hMPVGCHTPGrv+eGoM9IXWqsf4s/lMCnNl1R4QHpGeE8SjoFKM/n7ZdIlrNME8Q3A==;EndpointSuffix=core.windows.net";

            //db = new CloudStorageAccount(new StorageCredentials(storageAccountName, storageAccountKey), true);
        }

        public void temp()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageAccountKey);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("files");
            BlobClient blockBlob = containerClient.GetBlobClient("askdhjksaf".ToString() + ".jpg");

            if (blockBlob != null)
            {

            }
        }
        public string SaveFile(byte[] filebytes, bool createThumb, int thumbsize, int orjsize, string guid, string fileext, string blobContainerName)
        {
            int a = 1;
            try
            {
                /*string blobContainerName = "files";
                if (fileext == ".xls" || fileext == ".xlsx")
                {
                    blobContainerName = "takbisfiles";
                }*/

                a = 2;

                BlobServiceClient blobServiceClient = new BlobServiceClient(storageAccountKey);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
                BlobClient blockBlob = containerClient.GetBlobClient(guid.ToString() + fileext);

                MemoryStream fsThumb = new MemoryStream();
                MemoryStream fsOrj = new MemoryStream();

                if (fileext == ".png" || fileext == ".jpg")
                {
                    MemoryStream fsThumbFirst = new MemoryStream(filebytes);
                    MemoryStream fsOrjFirst = new MemoryStream(filebytes);

                    Bitmap bmpThumb = new Bitmap(fsThumbFirst);
                    Bitmap bmpOrj = new Bitmap(fsOrjFirst);

                    if (createThumb)
                    {
                        a = 3;
                        Bitmap thumb = Utility.CreateThumbnail(bmpThumb, thumbsize, thumbsize);
                        if (fileext == ".png")
                        {
                            thumb.Save(fsThumb, ImageFormat.Png);
                            //blockBlob.Properties.ContentType = "image/png";
                        }
                        else
                        {
                            thumb.Save(fsThumb, ImageFormat.Jpeg);
                            //blockBlob.Properties.ContentType = "image/jpeg";
                        }
                    }

                    a = 4;

                    Bitmap bmpSave = Utility.CreateThumbnail(bmpOrj, orjsize, orjsize);

                    a = 5;

                    if (fileext == ".png")
                    {
                        a = 6;
                        bmpSave.Save(fsOrj, ImageFormat.Png);
                    }
                    else
                    {
                        a = 7;
                        bmpSave.Save(fsOrj, ImageFormat.Jpeg);
                    }
                }
                else
                {
                    fsOrj = new MemoryStream(filebytes);
                    //blockBlob.Properties.ContentType = "application/octet-stream";
                }

                a = 8;

                //long size = fsOrj.Length;

                a = 9;

                fsOrj.Position = 0;

                a = 10;

                //blockBlob.UploadFromStream(fsOrj);
                if (fsOrj == null)
                {
                    a = 100;
                }
                else if (blockBlob == null)
                {
                    a = 101;
                }

                if (!blockBlob.Exists())
                {
                    blockBlob.Upload(fsOrj);
                }
                else
                {
                    blockBlob.Upload(fsOrj, true);
                }

                a = 11;

                fsOrj.Close();

                a = 12;

                if (createThumb)
                {
                    blockBlob = containerClient.GetBlobClient(guid.ToString() + "_thumb" + fileext); //container.GetBlockBlobReference(guid + "_thumb" + fileext);
                    fsThumb.Position = 0;
                    if (!blockBlob.Exists())
                    {
                        blockBlob.Upload(fsThumb);
                    }
                    else
                    {
                        blockBlob.Upload(fsThumb, true);
                    }
                    fsThumb.Close();
                }

                return guid.ToString();

            }
            catch (Exception ex)
            {
                return " a:" + a.ToString() + " guid: " + guid + " error: " + ex.Message;
            }
        }

    }
}
