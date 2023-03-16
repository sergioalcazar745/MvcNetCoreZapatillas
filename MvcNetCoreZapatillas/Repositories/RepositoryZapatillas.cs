using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreZapatillas.Data;
using MvcNetCoreZapatillas.Models;

#region SQL SERVER
//CREATE PROCEDURE SP_GRUPO_ZAPATILLAS_IMAGEN
//(@POSICION INT, @IDPRODUCTO NVARCHAR(50)
//, @NUMEROREGISTROS INT OUT)
//AS
//    SELECT @NUMEROREGISTROS = COUNT(IDIMAGEN) FROM
//    IMAGENESZAPASPRACTICA WHERE IDPRODUCTO = @IDPRODUCTO
//	SELECT IDIMAGEN, IDPRODUCTO, IMAGEN FROM
//		(SELECT CAST(
//			ROW_NUMBER() OVER(ORDER BY IDIMAGEN) AS INT) AS POSICION,
//            IDIMAGEN, IDPRODUCTO, IMAGEN
//		FROM IMAGENESZAPASPRACTICA
//		WHERE IDPRODUCTO = @IDPRODUCTO) AS QUERY
//	WHERE QUERY.POSICION >= @POSICION AND QUERY.POSICION < (@POSICION + 1)
//GO
#endregion

namespace MvcNetCoreZapatillas.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillas()
        {
            return await this.context.Zapatillas.ToListAsync();
        }

        public async Task<Zapatilla> GetZapatilla(int idproducto)
        {
            return await this.context.Zapatillas.FirstOrDefaultAsync(x => x.IdProducto == idproducto);
        }

        public async Task<ModelPaginarZapatilla> GetImagenZapatilla(int posicion, int idproducto)
        {
            string sql = "SP_GRUPO_ZAPATILLAS_IMAGEN @POSICION, @IDPRODUCTO, @NUMEROREGISTROS OUT";
            SqlParameter paraposicion = new SqlParameter("@POSICION", posicion);
            SqlParameter paraidproducto = new SqlParameter("@IDPRODUCTO", idproducto);
            SqlParameter paranumeroregistros = new SqlParameter("@NUMEROREGISTROS", -1);
            paranumeroregistros.Direction = System.Data.ParameterDirection.Output;
            var consulta = this.context.ImagenesZapatillas.FromSqlRaw(sql, paraposicion, paraidproducto, paranumeroregistros);
            var datos = consulta.AsEnumerable();
            ImagenZapatilla imagen = datos.FirstOrDefault();
            return new ModelPaginarZapatilla
            {
                ImagenZapatilla = imagen,
                NumeroRegistros = (int)paranumeroregistros.Value
            };
        }
    }
}
