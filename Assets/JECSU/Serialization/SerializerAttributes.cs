namespace JECSU.Serialization
{
    /// <summary>
    /// Indicates that this info wont be recognized when creating a template out of an entity
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct |
        System.AttributeTargets.Field | System.AttributeTargets.Property)
    ]
    public class TemplateIgnore : System.Attribute
    {
        public TemplateIgnore()
        {
        }
    }

}