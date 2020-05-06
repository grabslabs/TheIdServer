﻿using Aguacongas.IdentityServer.Store;
using Aguacongas.IdentityServer.Store.Entity;
using Aguacongas.TheIdServer.BlazorApp.Components.ExternalProviderComponents;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Aguacongas.TheIdServer.BlazorApp.Pages
{
    public partial class ExternalProvider
    {
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Used in component")]
        [SuppressMessage("Major", "CS0649:Fiel is never asign to", Justification = "Assign by jsScript.")]
        private ProviderOptionsBase _optionsComponent;

        protected override string Expand => $"{nameof(Models.ExternalProvider.ClaimTransformations)}";

        protected override bool NonEditable => false;

        protected override string BackUrl => "providers";

        protected override void SetNavigationProperty<TEntity>(TEntity entity)
        {
            // no nav
        }

        protected override Models.ExternalProvider Create()
        {
            return new Models.ExternalProvider
            {
                ClaimTransformations = new List<ExternalClaimTransformation>()
            };
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            var providerKindsResponse = await _providerKindStore.GetAsync(new PageRequest()).ConfigureAwait(false);
            Model.Kinds = providerKindsResponse.Items;
        }

        protected override void SanetizeEntityToSaved<TEntity>(TEntity entity)
        {
            if (entity is Models.ExternalProvider provider)
            {
                provider.SerializedOptions = _optionsComponent.SerializeOptions();
            }
        }

        protected override void OnEntityUpdated(Type entityType, IEntityId entityModel)
        {
            if (entityType != typeof(ExternalClaimTransformation))
            {
                base.OnEntityUpdated(typeof(Models.ExternalProvider), Model);
            }
            else
            {
                base.OnEntityUpdated(entityType, entityModel);
            }    
        }

        private ExternalClaimTransformation CreateTransformation()
        {
            return new ExternalClaimTransformation
            {
                Scheme = Model.Id
            };
        }
    }
}
