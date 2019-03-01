using System;
using System.Collections.Generic;
using System.Linq;
using OpenRpg.Editors.App.Extensions;
using OpenRpg.Editors.App.Models;
using OpenRpg.Genres.Fantasy.Types;

namespace OpenRpg.Editors.App.Helpers
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
        
        public static readonly OptionData[] GetItemTypes = GetTypesFor(typeof(ItemTypes));
        public static readonly OptionData[] GetItemQualityTypes = GetTypesFor(typeof(ItemQualityTypes));
        public static readonly OptionData[] GetRequirementTypes = GetTypesFor(typeof(RequirementTypes));
        public static readonly OptionData[] GetModificationTypes = GetTypesFor(typeof(ModificationTypes));
        public static readonly OptionData[] GetEffectTypes = GetTypesFor(typeof(EffectTypes));
        public static readonly OptionData[] GetRewardTypes = GetTypesFor(typeof(RewardTypes));
    }
}