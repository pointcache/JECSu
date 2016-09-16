namespace JECSU
{
    /// <summary>
    /// Indicates that this info wont be recognized when creating a template out of an entity
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct |
        System.AttributeTargets.Field | System.AttributeTargets.Property)
    ]
    public class JECSUAttribute : System.Attribute
    {
    }

    /// <summary>
    /// Indicates that this info wont be recognized when creating a template out of an entity
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct |
        System.AttributeTargets.Field | System.AttributeTargets.Property)
    ]
    public class TemplateIgnore : JECSUAttribute 
    {
    }
}