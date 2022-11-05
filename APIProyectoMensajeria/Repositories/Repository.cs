using Microsoft.EntityFrameworkCore;

namespace APIProyectoMensajeria.Repositories
{
    public class Repository<T> where T : class
    {
        //Creamos una propiedad que es nuestro DbContext
        public virtual DbContext Context { get; }
        //En el constructor pasamos como parametro un DbContext
        public Repository(DbContext context)
        {
            //Alimenatmos la propiedad Context con el parametro que recibimos del constructor
            Context = context;
        }
        //Metodo Insert para agregar un renglon en nuestra base de datos
        public virtual void Insert(T entidad)
        {
            Context.Add(entidad);
            Context.SaveChanges();
        }
        //Metodo GetAll que nos trae todos los datos de una tabla en especifico.
        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }
        //Metodo GetById que nos trae un solo renglon de una tabla
        public virtual T GetById(object id)
        {
            return Context.Find<T>(id);
        }
        //Metodo Update para editar un renglon en nuestra base de datos
        public virtual void Update(T entidad)
        {
            Context.Update(entidad);
            Context.SaveChanges();
        }
        //Metodo Delete para eliminar un renglon en nuestra base de datos
        public virtual void Delete(T entidad)
        {
            Context.Remove(entidad);
            Context.SaveChanges();
        }
    }
}
