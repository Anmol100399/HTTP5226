namespace PassionProject.Models.ViewModels
{
    public class OwnerEdit
    {
        // A Artist page must have a artwork
        // FindArtist(artistid)
        public required OwnerDto Owner { get; set; }

        // A Artist may have Artwork associated to it
        // ListArtworksForArtist(artistid)
        public IEnumerable<CarDto>? Cars { get; set; }

        // List to hold selected artwork IDs
        public List<int> SelectedCarId { get; set; }

    }
}
