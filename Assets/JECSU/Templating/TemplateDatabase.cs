using UnityEngine;
using System;
using System.Collections.Generic;

public interface ITemplateDatabase
{
    void Initialize(TemplateDatabaseConfig cfg);
}

public class TemplateDatabase  {

    TemplateDatabaseConfig cfg;
}
