using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HASmart.Core.Architecture;
using HASmart.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace HASmart.WebApi.Extensions {
    public static class ControllerExtensions {
        public static ActionResult HandleError(this ControllerBase controller, string property, string error) {
            ModelStateDictionary state = new ModelStateDictionary();
            state.AddModelError(property, error);
            return controller.ValidationProblem(state);
        }

        public static ActionResult HandleError(this ControllerBase controller, EntityConcurrencyException e) {
            return controller.HandleError(e.EntityType.Name, "Erro desconhecido. Favor tentar novamente");
        }

        public static ActionResult HandleError(this ControllerBase controller, EntityValidationException e) {
            ModelStateDictionary state = new ModelStateDictionary();
            foreach (ValidationResult error in e.Errors) {
                state.AddModelError(error.MemberNames.FirstOrDefault() ?? "", error.ErrorMessage);
            }
            return controller.ValidationProblem(state);
        }

        public static ActionResult HandleError(this ControllerBase controller, EntityNotFoundException e) {
            //return controller.HandleError(e.EntityType.Name, "Entidade não encontrada");
            return controller.NotFound();
        }
    }
}
