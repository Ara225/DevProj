using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataAccess
{
    public class S3Upload 
    {
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUWest2                              ;
        private static IAmazonS3 s3Client;

        public static async Task UploadFileAsync(Stream FileStream, string bucketName, string keyName)
        {
            s3Client = new AmazonS3Client(bucketRegion);
            var fileTransferUtility =
                                new TransferUtility(s3Client);
            // Option 1. Upload a file. The file name is used as the object key name.
            await fileTransferUtility.UploadAsync(FileStream, bucketName, keyName);
            Console.WriteLine("Upload 1 completed");
        }
    }
}
