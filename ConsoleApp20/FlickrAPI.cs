namespace ConsoleApp20;
using FlickrNet;

public static class FlickrAPI
{
    private static readonly Flickr _flickr = new Flickr("f02f73c09c7b90a77675f4d2691b41f7");
    private static readonly Random _random = new Random();
    
    public static async Task<List<string>> GetPhotoUrlAsync(string request)
    {
        var photoSearchOptions = new PhotoSearchOptions
        {
            Text = request,
            SortOrder = PhotoSearchSortOrder.Relevance
        };
        PhotoCollection photos = await _flickr.PhotosSearchAsync(photoSearchOptions);
        var listPhotos = photos.ToList();
        if (listPhotos.Count == 0)
        {
            return null;
        }

        var count = 5;
        List<string> resultList = new List<string>();
        var randomPhotos = _random.Next(0, listPhotos.Count-5);
        for (int i = 0; i < count; i++)
        {
            resultList.Add(listPhotos[randomPhotos+i].LargeUrl);
        }

        return resultList;
    }
}