using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public.Doc
{
    public interface ICustomNameGetter
    {
        String GetName(MemberInfo info);
    }
}
