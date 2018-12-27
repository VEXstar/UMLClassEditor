using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using UMLClassEditor.DrawElements.Arrows;
using UMLClassEditor.DrawElements.Blocks;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor
{
    public class Storage<T>:IEnumerable<T>
    {
        List<T> sList = new List<T>();


        public Storage(Stream context,EventBridge bridge,List<string> classes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            sList = (List<T>) formatter.Deserialize(context);
            foreach (var v in sList)
            {
                if (!(v is DependencyArrow))
                {
                    (v as UMLClassBox).initMenu(bridge);
                    (v as UMLClassBox).beforeLoadSets(classes);
                    continue;
                }
                 
                DependencyArrow arrow = v as DependencyArrow;
                UMLClassBox f = null;
                UMLClassBox s = null;
                foreach (var block in sList)
                {
                    if (!(block is UMLClassBox))
                    {
                        continue;
                    }
                    if ((block as UMLClassBox).getGuid() == arrow.getFGUID())
                    {
                        f = block as UMLClassBox;
                    }
                    else if((block as UMLClassBox).getGuid() == arrow.getSGUID())
                    {
                        s = block as UMLClassBox;
                    }
                    if(f!=null&&s!=null)
                        break;
                }
                arrow.setDependencyBeforeLoad(f,s);

            }
        }

        public Storage()
        {
        }

        public void Add(T s)
        {
            sList.Add(s);
        }
        public bool Remove(T s)
        {
          return sList.Remove(s);
        }
        public void Clear(Canvas canvas)
        {
            foreach (var val in sList)
            {
                if (val is IDrawable)
                {
                    ((IDrawable)val).removeGraphicFromCanvas(canvas);
                }
            }
            sList.Clear();
        }

        public void save(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, sList);
        }
        public IEnumerator<T> GetEnumerator()
        {
          return  sList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}