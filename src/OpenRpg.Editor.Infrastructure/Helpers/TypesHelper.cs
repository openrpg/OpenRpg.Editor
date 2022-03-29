using System;
using System.Collections.Generic;
using System.Linq;
using OpenRpg.Editor.Infrastructure.Extensions;
using OpenRpg.Editor.Infrastructure.Models;
using OpenRpg.Genres.Types;
using EffectTypes = OpenRpg.Genres.Fantasy.Types.EffectTypes;
using ItemQualityTypes = OpenRpg.Genres.Fantasy.Types.ItemQualityTypes;
using ItemTypes = OpenRpg.Genres.Fantasy.Types.ItemTypes;
using ModificationTypes = OpenRpg.Genres.Fantasy.Types.ModificationTypes;
using RequirementTypes = OpenRpg.Genres.Fantasy.Types.RequirementTypes;
using RewardTypes = OpenRpg.Genres.Fantasy.Types.RewardTypes;

namespace OpenRpg.Editor.Infrastructure.Helpers
{
    public static class TypesHelper
    {
        public static OptionData[] GetTypesFor(Type typesObject)
        {
            var optionData = new List<OptionData>();
            var fields = typesObject.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var property in fields)
            {
                var value = (int)property.GetValue(null);
                optionData.Add(new OptionData(value, property.Name.MakeReadable()));
            }

            return optionData.OrderBy(x => x.Name).ToArray();
        }
        
        public static readonly OptionData[] GetItemTypes = GetTypesFor(typeof(ItemTypes)).OrderBy(x => x.Id).ToArray();
        public static readonly OptionData[] GetItemQualityTypes = GetTypesFor(typeof(ItemQualityTypes)).OrderBy(x => x.Id).ToArray();
        public static readonly OptionData[] GetRequirementTypes = GetTypesFor(typeof(RequirementTypes)).OrderBy(x => x.Id).ToArray();
        public static readonly OptionData[] GetModificationTypes = GetTypesFor(typeof(ModificationTypes)).OrderBy(x => x.Id).ToArray();
        public static readonly OptionData[] GetEffectTypes = GetTypesFor(typeof(EffectTypes)).OrderBy(x => x.Id).ToArray();
        public static readonly OptionData[] GetRewardTypes = GetTypesFor(typeof(RewardTypes)).OrderBy(x => x.Id).ToArray();
        public static readonly OptionData[] GetObjectiveTypes = GetTypesFor(typeof(ObjectiveTypes)).OrderBy(x => x.Id).ToArray();
    }
}