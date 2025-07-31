using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using PathFinder.Data.Models.DTOs;
using PathFinder.Server.Models;
using PathFinder.Server.Services.Interfaces;
using Directory = Lucene.Net.Store.Directory;

namespace PathFinder.Server.Services;

public class CitySearchService : ICitySearchService, IDisposable
{
    private const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
    private readonly Directory _directory;
    private readonly IndexWriter _writer;
    private readonly Lucene.Net.Analysis.Analyzer _analyzer;
    
    public CitySearchService(string indexPath)
    {
        _directory = FSDirectory.Open(indexPath);
        _analyzer = new StandardAnalyzer(AppLuceneVersion);
        var indexConfig = new IndexWriterConfig(AppLuceneVersion, _analyzer);
        _writer = new IndexWriter(_directory, indexConfig);
    }
    
    public void IndexCities(IEnumerable<GtfsFeedResponseDto> feeds)
    {
        foreach (var feed in feeds)
        {
            var doc = new Document
            {
                new StringField("id", feed.Id, Field.Store.YES),
                new TextField("provider", feed.Provider, Field.Store.YES),
                //TODO dar kazkaip nested list of Locations store'int
            };
            _writer.AddDocument(doc);
        }
        _writer.Commit();
    }
    
    public List<CitySearchResult> SearchCities(string searchTerm, int limit = 20)
    {
        using var reader = _writer.GetReader(true);
        var searcher = new IndexSearcher(reader);
        
        var query = new FuzzyQuery(new Term("provider", searchTerm), 2);
        var hits = searcher.Search(query, limit).ScoreDocs;
        
        return hits.Select(hit =>
        {
            var doc = searcher.Doc(hit.Doc);
            return new CitySearchResult
            {
                Id = doc.Get("id"),
                Provider = doc.Get("provider"),
                // City = doc.Get("name"),
                // CountryCode = doc.Get("country")
            };
        }).ToList();
    }
    
    public void Dispose()
    {
        _writer?.Dispose();
        _directory?.Dispose();
        _analyzer?.Dispose();
    }
}