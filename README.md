# Azure Blob Storage Gallery
A simple azure blob storage example in the form of a photo gallery. This app provides the ability to view any images in the storage container, upload new images and delete images.

## Technologies used
- ASP.NET MVC 5
- .NET 4.7.2
- Azure Storage

## Running this sample
1. You must replace the connection string with the values of an active Azure Storage Account. If you don't have an account, refer to the [Create a Storage Account](https://azure.microsoft.com/en-us/documentation/articles/storage-create-storage-account/) article.

2. Retrieve the STORAGE ACCOUNT NAME and PRIMARY ACCESS KEY (or SECONDARY ACCESS KEY) values from the Keys blade of your Storage account in the Azure Preview portal. For more information on obtaining keys for your Storage account refer to [View, copy, and regenerate storage access keys](https://azure.microsoft.com/en-us/documentation/articles/storage-create-storage-account/#view-copy-and-regenerate-storage-access-keys

3. In the **Web.config** file, located in the project root, find the **StorageAccount** app setting and replace the placeholder values with the values obtained for your account.

  <add key="StorageAccount" value="DefaultEndpointsProtocol=https;AccountName=[Enter Your Storage AccountName];AccountKey=[Enter Your Storage AccountKey]" />
  
## More information
- [What is a Storage Account](http://azure.microsoft.com/en-us/documentation/articles/storage-whatis-account/)
- [Getting Started with Blobs](http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/)
- [Blob Service Concepts](http://msdn.microsoft.com/en-us/library/dd179376.aspx)
- [Blob Service REST API](http://msdn.microsoft.com/en-us/library/dd135733.aspx)
- [Blob Service C# API](http://go.microsoft.com/fwlink/?LinkID=398944)
- [Delegating Access with Shared Access Signatures](http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-shared-access-signature-part-1/)