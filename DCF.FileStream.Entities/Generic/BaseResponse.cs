using System.Text.Json.Serialization;

namespace DCF.FileStream.Entities.Generic
{
    public class BaseResponse
    {
        /// <summary>
        /// Codigo de respuesta
        /// Valores soportados:<br/>
        /// 00 => Procesado satisfactoriamente<br/>
        /// 01 => No aplica<br/>
        /// 02 => No aplica<br/>
        /// 03 => No aplica<br/>
        /// 04 => No aplica<br/>
        /// 16 => No se encontro un objeto<br/>
        /// 22 => No se encontraron objetos para una lista<br/>
        /// 80 => No aplica<br/>
        /// 81 => No aplica<br/>
        /// 88 => No aplica<br/>
        /// 89 => Asociado a un error al intentar consumir un endpoint<br/>
        /// 99 => Excepción producto de un try catch
        /// </summary>
        public string CodResp { get; set; } = string.Empty;
        /// <summary>
        /// Descripcion relacionada al codigo de respuesta
        /// </summary>
        public string DesResp { get; set; } = string.Empty;
        /// <summary>
        /// Fecha y hora de inicio de la ejecución
        /// </summary>
        [JsonIgnore]
        public DateTime StartExec { get; set; } = DateTime.Now;
        /// <summary>
        /// Fecha y hora de finalización de la ejecución
        /// </summary>
        [JsonIgnore]
        public DateTime EndExec { get; set; } = DateTime.Now;
        /// <summary>
        /// Duración del procesamiento
        /// </summary>
        [JsonIgnore]
        public string Duration { get; set; } = string.Empty;
    }
}
