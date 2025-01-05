namespace Movies.Blazor.Models;

public record Movie
{
    public string Id { get; init; } // Movie ID
    public string Title { get; init; } // Movie title
    public string Genre { get; init; } // Movie genre
    public int ReleaseYear { get; init; } // Release year
    public double? Rating { get; init; } // Optional movie rating
    public List<string>? Cast { get; init; } // Optional list of actors
    public string? Director { get; init; } // Optional director's name
    public int? DurationMinutes { get; init; } // Optional movie duration in minutes
    public string? Language { get; init; } // Optional language
    public SpecialFeatures? SpecialFeatures { get; init; } // Optional special features
    public List<string>? Awards { get; init; } // Optional awards
    public List<Review>? Reviews { get; init; } // Optional reviews
    public double? BoxOfficeMillionUSD { get; init; } // Optional box office earnings
    public string? TrailerLink { get; init; } // Optional trailer link
}

// Supporting types
public record SpecialFeatures
{
    public bool? Has4K { get; init; } // Optional flag for 4K availability
    public bool? HasSubtitles { get; init; } // Optional flag for subtitles
    public List<string>? Audio { get; init; } // Optional list of audio features
    public string? Commentary { get; init; } // Optional commentary description
}

public record Review
{
    public string User { get; init; } // Reviewer's username
    public string Comment { get; init; } // Review comment
    public int Rating { get; init; } // Review rating
}
