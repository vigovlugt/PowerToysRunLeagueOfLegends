using System.IO;
using System.Reflection;
using Wox.Plugin;
using Wox.Plugin.Logger;
using System.Text.Json;
using Wox.Infrastructure;
using Wox.Plugin.Common;

namespace LeagueOfLegends;

public class Main : IPlugin
{
    public static string PluginID => "4TKXYW13BHLN5BMGNV3H261J4JUYWS7I";
    public string Name => "LeagueOfLegends";
    public string Description => "Quickly open League of Legends builds";
    private ChampionDataset? _dataset;

    private PluginInitContext _context;

    public void Init(PluginInitContext context)
    {
        _context = context;
    }

    public ChampionDataset GetDataset()
    {
        if (_dataset == null)
        {
            var currentDir = Assembly.GetExecutingAssembly().Location;
            var path = Path.Combine(Path.GetDirectoryName(currentDir)!, "champion.json");
            var text = File.ReadAllText(path);
            _dataset = JsonSerializer.Deserialize<ChampionDataset>(text, new JsonSerializerOptions
                       {
                           PropertyNamingPolicy =
                               JsonNamingPolicy.CamelCase
                       }) ??
                       throw new Exception(
                           "Failed to load champion dataset");
        }

        return _dataset;
    }

    public List<Result> Query(Query query)
    {
        var dataset = GetDataset();

        var search = query.Search;
        if (string.IsNullOrWhiteSpace(search))
        {
            return new List<Result>
            {
                new()
                {
                    IcoPath = _context.CurrentPluginMetadata.IcoPathDark,
                    Title = "Search for a champion",
                }
            };
        }

        var parts = search.Split(' ');

        var champions = dataset.Data.Values
            .Where(c => c.Id.StartsWith(parts[0], StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(c => c.Id.Length)
            .ToList();

        var idByMode = new Dictionary<string, string>
        {
            {"ofa", "oneforall"},
            {"oneforall", "oneforall"},
            {"aram", "aram"},
            {"urf", "urf"},
            {"arena", "arena"}
        };

        var idByRole = new Dictionary<string, string>
        {
            {"top", "top"},
            {"jungle", "jungle"},
            {"mid", "middle"},
            {"middle", "middle"},
            {"bot", "bottom"},
            {"bottom", "bottom"},
            {"adc", "bottom"},
            {"support", "support"},
            {"supp", "support"}
        };

        var chosenRole = parts.Where(p => idByRole.ContainsKey(p.ToLower())).Select(p => idByRole[p]).FirstOrDefault();
        var chosenMode = parts.Where(p => idByMode.ContainsKey(p.ToLower())).Select(p => idByMode[p]).FirstOrDefault();

        return champions.Select(c =>
            new Result
            {
                IcoPath = _context.CurrentPluginMetadata.IcoPathDark,
                Action = _ =>
                {
                    var id = c.Id;
                    if (id == "MonkeyKing")
                    {
                        id = "Wukong";
                    }

                    var url = $"https://lolalytics.com/lol/{id.ToLower()}/build/";
                    if (chosenMode != null)
                    {
                        url = $"https://lolalytics.com/lol/{id.ToLower()}/{chosenMode}/build/";
                    }

                    if (chosenRole != null)
                    {
                        url += $"?lane={chosenRole}";
                    }

                    if (!Helper.OpenCommandInShell(DefaultBrowserInfo.Path, DefaultBrowserInfo.ArgumentsPattern, url))
                    {
                        return false;
                    }

                    return true;
                },
                Score = c.Id.Equals(search, StringComparison.CurrentCultureIgnoreCase) ? 1000 : 0,
                Title = c.Name + (chosenMode != null ? $" {chosenMode}" : "") +
                        (chosenRole != null ? $" {chosenRole}" : "") + " Build"
            }
        ).ToList();
    }
}

public class ChampionDataset
{
    public Dictionary<string, Champion> Data { get; set; }
}

public class Champion
{
    public string Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
}