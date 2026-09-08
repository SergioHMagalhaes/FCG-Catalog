using System.Security.Cryptography;
using System.Text;

namespace FCG.Catalog.Domain.Constants;

public static class CacheKeys
{
    public const string Prefix = "fcg:catalog";

    public static class Games
    {
        private const string Root = $"{Prefix}:games";

        public static string ById(long id) => $"{Root}:id:{id}";

        public static string ListPrefix => $"{Root}:list:";

        public static string List(int page, int pageSize, int orderBy, bool desc, string? search)
        {
            var rawKey = $"page={page}&size={pageSize}&order={orderBy}&desc={desc}&search={search?.Trim().ToLowerInvariant()}";
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawKey));
            var hash = Convert.ToHexString(hashBytes).ToLowerInvariant()[..16];

            return $"{ListPrefix}{hash}";
        }
    }

    public static class Categories
    {
        private const string Root = $"{Prefix}:categories";

        public static string All => $"{Root}:all";

        public static string ById(long id) => $"{Root}:id:{id}";

        public static string PrefixAll => $"{Root}:";
    }
}
