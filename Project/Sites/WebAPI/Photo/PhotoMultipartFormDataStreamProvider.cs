using System;
using System.IO;
using System.Net.Http;

namespace WebAPI.Photo
{
	public class PhotoMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
	{
		public PhotoMultipartFormDataStreamProvider(string path)
			: base(path)
		{
		}

		public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
		{
			string oldfileName = headers.ContentDisposition.FileName.Replace("\"", string.Empty);
			string newFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(oldfileName);
			return newFileName;
		}
	}
}