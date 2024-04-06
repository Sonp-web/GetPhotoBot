using Telegram.Bot.Types;

namespace ConsoleApp20;
using FlickrNet;

public static class FlickrAPI
{
    private static readonly Flickr _flickr = new Flickr("f02f73c09c7b90a77675f4d2691b41f7");
    private static readonly Random _random = new Random();
    private const int COUNT = 5;
    
    public static async Task<IAlbumInputMedia[]> GetPhotoUrlAsync(string request)
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

        IAlbumInputMedia[] inputMedia = new IAlbumInputMedia[COUNT];
        var randomPhotos = _random.Next(0, listPhotos.Count-5);
        
        for (int i = 0; i < COUNT; i++)
        {
            inputMedia[i] = new InputMediaPhoto(new InputFileUrl(listPhotos[randomPhotos + i].LargeUrl));
        }

        
        
        return inputMedia;
    }
}