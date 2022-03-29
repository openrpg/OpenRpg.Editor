using OpenRpg.Core.Classes;
using OpenRpg.Core.Common;
using OpenRpg.Core.Races;
using OpenRpg.Items.Templates;
using OpenRpg.Localization.Data.Extensions;
using OpenRpg.Localization.Data.Repositories;
using OpenRpg.Quests;

namespace OpenRpg.Editor.Infrastructure.Extensions
{
    public static class IHasLocaleDescriptionExtensions
    {
        public static void GenerateLocaleCodes<T>(this T localeEntity, string newAssetCode) where T : IHasLocaleDescription
        {
            if (localeEntity is DefaultItemTemplate itemTemplate)
            {
                itemTemplate.NameLocaleId = $"{newAssetCode}-name";
                itemTemplate.DescriptionLocaleId = $"{newAssetCode}-description";
            }
            else if (localeEntity is DefaultClassTemplate classTemplate)
            {
                classTemplate.NameLocaleId = $"{newAssetCode}-name";
                classTemplate.DescriptionLocaleId = $"{newAssetCode}-description";
            }
            else if (localeEntity is DefaultRaceTemplate raceTemplate)
            {
                raceTemplate.NameLocaleId = $"{newAssetCode}-name";
                raceTemplate.DescriptionLocaleId = $"{newAssetCode}-description";
            }
            else if (localeEntity is DefaultQuest quest)
            {
                quest.NameLocaleId = $"{newAssetCode}-name";
                quest.DescriptionLocaleId = $"{newAssetCode}-description";
            }
        }

        public static void UpdateLocale<T>(this T localeEntity, string newAssetCode, ILocaleRepository repository)
            where T : IHasLocaleDescription
        {
            var oldNameLocaleId = localeEntity.NameLocaleId;
            var oldDescriptionLocaleId = localeEntity.DescriptionLocaleId;

            localeEntity.GenerateLocaleCodes(newAssetCode);
            
            MoveLocaleInRepository(repository, oldNameLocaleId, localeEntity.NameLocaleId);
            MoveLocaleInRepository(repository, oldDescriptionLocaleId, localeEntity.DescriptionLocaleId);
        }

        public static void MoveLocaleInRepository(this ILocaleRepository repository, string oldLocaleKey, string newLocaleKey)
        {
            if (!repository.Exists(oldLocaleKey)) { return; }
            var localeData = repository.Get(oldLocaleKey);
            repository.Delete(oldLocaleKey);
            repository.Create(newLocaleKey, localeData);
        }
    }
}