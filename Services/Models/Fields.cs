using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneConnector.Services.Models
{
    public class ContentClass : BaseField<string> { }
    public class CategoryName : BaseField<string> { }
    public class Subject : BaseField<string> { }
    public class DetailsSubject : BaseField<string> { }
    public class Company : BaseField<string> { }
    public class UID : BaseField<string> { }
    public class Modified : BaseField<System.DateTime> { }
    public class CreateDate : BaseField<System.DateTime> { }
    public class ItemID : BaseField<string> { }
    public class ModifiedBy : BaseField<string> { }
    public class Status : BaseField<string> { }
    public class Author : BaseField<string> { }
    public class Creator : BaseField<string> { }
    public class DocCategory : BaseField<string> { }
    public class DocType : BaseField<string> { }
}
