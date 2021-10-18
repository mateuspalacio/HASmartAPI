using System;
using HASmart.Core.Architecture;
using System.ComponentModel.DataAnnotations;
using HASmart.Core.Validation;


namespace HASmart.Core.Entities  {
    public enum Macroregiao { 
        Invalido =0,
        Cariri = 1, 
        Centro_Sul = 2, CentroSul = 2, 
        Grande_Fortaleza=3 , GrandeFortaleza=3, 
        Litoral_Leste=4, LitoralLeste=4,
        Litoral_Norte=5, LitoralNorte=5,
        Litoral_Oeste=6, Vale_do_Curu=6, LitoralOeste=6, ValeDoCuru=6,
        Maciço_de_Baturité=7, MaciçoDeBaturite=7,
        Serra_da_Ibiapaba=8 ,SerraDaIbiapaba=8,
        Sertão_Central=9,SertaoCentral=9,
        Sertão_de_Canindé=10,SertaoDeCaninde=10,
        Sertão_de_Cratéus=11,SertaoDeCrateus=11,
        Sertão_de_Sobral=12,SertaoDeSobral=12,
        Sertão_dos_Inhamuns=13,SertaoDosInhamuns=13,
        Vale_do_Jaguaribe=14,ValeDoJaguaribe=14
    }
    public class EnderecoFarmacia : Endereco {
        public const string mensagemErroMacroregião= "O campo Macroregião é obrigatório";
        public const string mensagemErroValidaçãoMacroregião= "O campo Macroregião é um valor de 1 a 14 que representa uma macroregião específica ao numero";
        [Required(ErrorMessage =  mensagemErroMacroregião)]
        [IsValidEnumValue(typeof(Macroregiao), ErrorMessage = mensagemErroValidaçãoMacroregião)]
        public Macroregiao Macroregiao{ get; set; }
    }
}
