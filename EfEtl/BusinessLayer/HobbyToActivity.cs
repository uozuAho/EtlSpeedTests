using EfEtl.Models;
using System.Collections.Generic;

namespace EfEtl.BusinessLayer
{
    public class HobbyToActivity
    {
        public static Activity NewActivity(EfEtl_Hobby hobby)
        {
            return new Activity
            {
                HobbyId = hobby.Id,
                Name = hobby.Name,
            };
        }

        public static IEnumerable<Property> GetActivityProperties(EfEtl_Hobby hobby, int activityId)
        {
            var targetData = TargetDbData.GetInstance();
            yield return new Property
            {
                PropertyTypeId = targetData.PropertyTypes["Hobby Id"].Id,
                ActivityId = activityId,
                Value = hobby.Id == null ? null : hobby.Id.Value + ""
            };
            yield return new Property
            {
                PropertyTypeId = targetData.PropertyTypes["Hobby Type"].Id,
                ActivityId = activityId,
                Value = hobby.Type
            };
        }
    }
}
