using System;

namespace Vault.Core.Models;

public class SearchResult
{
    public string Id{get; set;} = null!;
    public string Path{get; set;} = null!;
    public string Snippet {get; set;} = null!;
    public int PageNumber{get; set;}
}
