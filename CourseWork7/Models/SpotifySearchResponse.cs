#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork7.Models
{
    internal class SpotifySearchResponse
    {

        public class Rootobject
        {
            public string status { get; set; }
            public Albums albums { get; set; }
            public Artists1 artists { get; set; }
            public Episodes episodes { get; set; }
            public Genres genres { get; set; }
            public Playlists playlists { get; set; }
            public Podcasts podcasts { get; set; }
            public Topresults topResults { get; set; }
            public Tracks tracks { get; set; }
            public Users users { get; set; }
        }

        public class Albums
        {
            public int totalCount { get; set; }
            public Item[] items { get; set; }
        }

        public class Item
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public string uri { get; set; }
            public string name { get; set; }
            public Artists artists { get; set; }
            public Coverart coverArt { get; set; }
            public Date date { get; set; }
        }

        public class Artists
        {
            public Item1[] items { get; set; }
        }

        public class Item1
        {
            public string uri { get; set; }
            public Profile profile { get; set; }
        }

        public class Profile
        {
            public string name { get; set; }
        }

        public class Coverart
        {
            public Source[] sources { get; set; }
        }

        public class Source
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Date
        {
            public int year { get; set; }
        }

        public class Artists1
        {
            public int totalCount { get; set; }
            public Item2[] items { get; set; }
        }

        public class Item2
        {
            public Data1 data { get; set; }
        }

        public class Data1
        {
            public string uri { get; set; }
            public Profile1 profile { get; set; }
            public Visuals visuals { get; set; }
        }

        public class Profile1
        {
            public string name { get; set; }
        }

        public class Visuals
        {
            public Avatarimage avatarImage { get; set; }
        }

        public class Avatarimage
        {
            public Source1[] sources { get; set; }
        }

        public class Source1
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Episodes
        {
            public int totalCount { get; set; }
            public Item3[] items { get; set; }
        }

        public class Item3
        {
            public Data2 data { get; set; }
        }

        public class Data2
        {
            public string uri { get; set; }
            public string name { get; set; }
            public Coverart1 coverArt { get; set; }
            public Duration duration { get; set; }
            public Releasedate releaseDate { get; set; }
            public Podcast podcast { get; set; }
            public string description { get; set; }
            public Contentrating contentRating { get; set; }
        }

        public class Coverart1
        {
            public Source2[] sources { get; set; }
        }

        public class Source2
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Duration
        {
            public int totalMilliseconds { get; set; }
        }

        public class Releasedate
        {
            public DateTime isoString { get; set; }
        }

        public class Podcast
        {
            public Coverart2 coverArt { get; set; }
        }

        public class Coverart2
        {
            public Source3[] sources { get; set; }
        }

        public class Source3
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Contentrating
        {
            public string label { get; set; }
        }

        public class Genres
        {
            public int totalCount { get; set; }
            public Item4[] items { get; set; }
        }

        public class Item4
        {
            public Data3 data { get; set; }
        }

        public class Data3
        {
            public string uri { get; set; }
            public string name { get; set; }
            public Image image { get; set; }
        }

        public class Image
        {
            public Source4[] sources { get; set; }
        }

        public class Source4
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Playlists
        {
            public int totalCount { get; set; }
            public Item5[] items { get; set; }
        }

        public class Item5
        {
            public Data4 data { get; set; }
        }

        public class Data4
        {
            public string uri { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public Images images { get; set; }
            public Owner owner { get; set; }
        }

        public class Images
        {
            public Item6[] items { get; set; }
        }

        public class Item6
        {
            public Source5[] sources { get; set; }
        }

        public class Source5
        {
            public string url { get; set; }
            public int? width { get; set; }
            public int? height { get; set; }
        }

        public class Owner
        {
            public string name { get; set; }
        }

        public class Podcasts
        {
            public int totalCount { get; set; }
            public Item7[] items { get; set; }
        }

        public class Item7
        {
            public Data5 data { get; set; }
        }

        public class Data5
        {
            public string uri { get; set; }
            public string name { get; set; }
            public Coverart3 coverArt { get; set; }
            public string type { get; set; }
            public Publisher publisher { get; set; }
            public string mediaType { get; set; }
        }

        public class Coverart3
        {
            public Source6[] sources { get; set; }
        }

        public class Source6
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Publisher
        {
            public string name { get; set; }
        }

        public class Topresults
        {
            public Item8[] items { get; set; }
            public Featured[] featured { get; set; }
        }

        public class Item8
        {
            public Data6 data { get; set; }
        }

        public class Data6
        {
            public string uri { get; set; }
            public Profile2 profile { get; set; }
            public Visuals1 visuals { get; set; }
            public string name { get; set; }
            public Coverart4 coverArt { get; set; }
            public Duration1 duration { get; set; }
            public Releasedate1 releaseDate { get; set; }
            public Podcast1 podcast { get; set; }
            public string description { get; set; }
            public Contentrating1 contentRating { get; set; }
            public string id { get; set; }
            public Albumoftrack albumOfTrack { get; set; }
            public Artists2 artists { get; set; }
            public Playability playability { get; set; }
            public Images1 images { get; set; }
            public Owner1 owner { get; set; }
        }

        public class Profile2
        {
            public string name { get; set; }
        }

        public class Visuals1
        {
            public Avatarimage1 avatarImage { get; set; }
        }

        public class Avatarimage1
        {
            public Source7[] sources { get; set; }
        }

        public class Source7
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Coverart4
        {
            public Source8[] sources { get; set; }
        }

        public class Source8
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Duration1
        {
            public int totalMilliseconds { get; set; }
        }

        public class Releasedate1
        {
            public DateTime isoString { get; set; }
        }

        public class Podcast1
        {
            public Coverart5 coverArt { get; set; }
        }

        public class Coverart5
        {
            public Source9[] sources { get; set; }
        }

        public class Source9
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Contentrating1
        {
            public string label { get; set; }
        }

        public class Albumoftrack
        {
            public string uri { get; set; }
            public string name { get; set; }
            public Coverart6 coverArt { get; set; }
            public string id { get; set; }
            public Sharinginfo sharingInfo { get; set; }
        }

        public class Coverart6
        {
            public Source10[] sources { get; set; }
        }

        public class Source10
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Sharinginfo
        {
            public string shareUrl { get; set; }
        }

        public class Artists2
        {
            public Item9[] items { get; set; }
        }

        public class Item9
        {
            public string uri { get; set; }
            public Profile3 profile { get; set; }
        }

        public class Profile3
        {
            public string name { get; set; }
        }

        public class Playability
        {
            public bool playable { get; set; }
        }

        public class Images1
        {
            public Item10[] items { get; set; }
        }

        public class Item10
        {
            public Source11[] sources { get; set; }
        }

        public class Source11
        {
            public string url { get; set; }
            public object width { get; set; }
            public object height { get; set; }
        }

        public class Owner1
        {
            public string name { get; set; }
        }

        public class Featured
        {
            public Data7 data { get; set; }
        }

        public class Data7
        {
            public string uri { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public Images2 images { get; set; }
            public Owner2 owner { get; set; }
        }

        public class Images2
        {
            public Item11[] items { get; set; }
        }

        public class Item11
        {
            public Source12[] sources { get; set; }
        }

        public class Source12
        {
            public string url { get; set; }
            public object width { get; set; }
            public object height { get; set; }
        }

        public class Owner2
        {
            public string name { get; set; }
        }

        public class Tracks
        {
            public int totalCount { get; set; }
            public Item12[] items { get; set; }
        }

        public class Item12
        {
            public Data8 data { get; set; }
        }

        public class Data8
        {
            public string uri { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public Albumoftrack1 albumOfTrack { get; set; }
            public Artists3 artists { get; set; }
            public Contentrating2 contentRating { get; set; }
            public Duration2 duration { get; set; }
            public Playability1 playability { get; set; }
        }

        public class Albumoftrack1
        {
            public string uri { get; set; }
            public string name { get; set; }
            public Coverart7 coverArt { get; set; }
            public string id { get; set; }
            public Sharinginfo1 sharingInfo { get; set; }
        }

        public class Coverart7
        {
            public Source13[] sources { get; set; }
        }

        public class Source13
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Sharinginfo1
        {
            public string shareUrl { get; set; }
        }

        public class Artists3
        {
            public Item13[] items { get; set; }
        }

        public class Item13
        {
            public string uri { get; set; }
            public Profile4 profile { get; set; }
        }

        public class Profile4
        {
            public string name { get; set; }
        }

        public class Contentrating2
        {
            public string label { get; set; }
        }

        public class Duration2
        {
            public int totalMilliseconds { get; set; }
        }

        public class Playability1
        {
            public bool playable { get; set; }
        }

        public class Users
        {
            public int totalCount { get; set; }
            public Item14[] items { get; set; }
        }

        public class Item14
        {
            public Data9 data { get; set; }
        }

        public class Data9
        {
            public string uri { get; set; }
            public string id { get; set; }
            public string displayName { get; set; }
            public string username { get; set; }
            public Image1 image { get; set; }
        }

        public class Image1
        {
            public string smallImageUrl { get; set; }
            public string largeImageUrl { get; set; }
        }

    }
}
