using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace HASmart.Core.Extensions {
    public static class EnumExtensions {
        /// <summary>
        /// Conversão custom de string para enum, evitando o valor "Invalido" e convertendo espaço para underline
        /// </summary>
        /// <param name="enumType">O tipo do enum que será convertido</param>
        /// <param name="value">A string que contém o valor do enum</param>
        /// <param name="result">O out-retorno com o valor convertido</param>
        /// <returns></returns>
        public static bool TryParse(Type enumType, string value, out object result) {
            if (!enumType.IsEnum) {
                throw new ArgumentException($"{nameof(enumType)} deve ser do tipo Enum");
            }

            if (Enum.TryParse(enumType, value.Replace(' ', '_'), ignoreCase: true, out object intermediate)) {
                result = intermediate;
                // Só dê sucesso se o valor lido não for o valor "Invalido"
                return (int)result != 0;
            } else {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Versão não checada e type-safe da conversão de string para enum.
        /// Esse método não faz nenhuma checagem de erros, é assumido que a string é válida
        /// </summary>
        /// <typeparam name="T">O tipo Enum desejado</typeparam>
        /// <param name="value">A string com o valor a ser decodificado em um enum</param>
        /// <returns></returns>
        public static T UncheckedParse<T>(string value) where T : struct {
            return Enum.Parse<T>(value.Replace(' ', '_'), ignoreCase: true);
        }

        /// <summary>
        /// Lista todos os valores que o Enum pode assumir (menos o inválido) na forma CSV
        /// </summary>
        /// <param name="type">O tipo Enum que se deseja listar todos os valores</param>
        /// <returns></returns>
        public static string ListStrings(Type type) {
            if (!type.IsEnum) {
                throw new ArgumentException($"{nameof(type)} deve ser do tipo Enum");
            }

            StringBuilder sb = new StringBuilder();

            // Começa o for de 1 para pular o "Invalido" dos enums.
            string[] names = Enum.GetNames(type);
            for (int index = 1; index < names.Length - 1; index++) {
                sb.Append(names[index]);
                sb.Append(", ");
            }

            // Adiciona o último elemento sem por um ", " extra
            sb.Append(names[^1]);
            return sb.ToString();
        }

        /// <summary>
        /// Lista todos os valores que o Enum pode assumir (menos o inválido) na forma CSV
        /// </summary>
        /// <param name="type">O tipo Enum que se deseja listar todos os valores</param>
        /// <returns></returns>
        public static string ListValues(Type type) {
            if (!type.IsEnum) {
                throw new ArgumentException($"{nameof(type)} deve ser do tipo Enum");
            }

            StringBuilder sb = new StringBuilder();

            // Começa o for de 1 para pular o "Invalido" dos enums.
            int[] values = Enum.GetValues(type).Cast<int>().Distinct().ToArray();
            for (int index = 1; index < values.Length - 1; index++) {
                int v = values[index];
                sb.Append($"{v}: {Enum.GetName(type, v)}");
                sb.Append(", ");
            }

            // Adiciona o último elemento sem por um ", " extra
            sb.Append($"{values[^1]}: {Enum.GetName(type, values[^1])}");
            return sb.ToString();
        }
    }
}
