using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.ActivateEquipmentUser;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentAssign;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentReport;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.DeleteEquipmentUser;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetAssignsByEquipmentId;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetEquipmentUsersLookup;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipments;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipmentsWithFilterOptions;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentActiveHolder;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentApprovers;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsEquipmentUserFromDepartment;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsSerialNumberExisting;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.SetReturnDateToAssign;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.UpdateEquipmentDbQuery;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Builders;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.EquipmentReportQueries;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.GetPageableEquipments;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.Extensions;

public static class DbQueryServicesExtensions
{
    internal static IServiceCollection AddDbQueries(this IServiceCollection services)
    {
        services.AddScoped<IIsEquipmentUserFromDepartmentDbQuery, IsEquipmentUserFromDepartmentDbQuery>();
        services.AddScoped<IIsUserWithActiveEquipmentAssignmentsDbQuery, IsUserWithActiveEquipmentAssignmentsDbQuery>();
        services.AddScoped<IIsUserApproverOfEquipmentDbQuery,
            IsUserApproverOfEquipmentDbQuery>();

        services.AddScoped<IDeactivateEquipmentUserDbQuery, DeactivateEquipmentUserDbQueryDbQuery>();
        services.AddScoped<IActivateEquipmentUserDbQuery, ActivateEquipmentUserDbQuery>();

        services.AddScoped<IGetEquipmentTypesDbQuery, GetEquipmentTypesDbQuery>();
        services.AddScoped<IIsEquipmentTypeExistDbQuery, IsEquipmentTypeExistDbQuery>();
        services.AddScoped<IGetEquipmentTypeByIdDbQuery, GetEquipmentTypeByIdDbQuery>();

        services.AddScoped<ICreateEquipmentReportDbQuery, CreateEquipmentReportDbQuery>();
        services.AddScoped<IUpdateEquipmentReportRelevancePeriodDbQuery, UpdateEquipmentReportRelevancePeriodDbQuery>();
        services.AddScoped<IGetEquipmentReportBySerialNumberAndDataHashDbQuery, GetEquipmentReportBySerialNumberAndDataHashDbQuery>();
        services.AddScoped<IGetEquipmentReportByIdDbQuery, GetEquipmentReportByIdDbQuery>();
        services.AddScoped<IGetEquipmentReportRelevancePeriodsDbQuery, GetEquipmentReportRelevancePeriodsDbQuery>();
        services.AddScoped<IIsEquipmentExistingBySerialNumberDbQuery, IsEquipmentExistingBySerialNumberDbQuery>();
        services.AddScoped<IIsEquipmentExistByIdDbQuery, IsEquipmentExistByIdDbQuery>();
        services.AddScoped<ICreateEquipmentAssignDbQuery, CreateEquipmentAssignDbQuery>();
        services.AddScoped<IGetEquipmentUserByIdDbQuery, GetEquipmentUserByIdDbQuery>();
        services.AddScoped<IGetAssignsReturnDatesByEquipmentIdDbQuery, GetAssignsReturnDatesByEquipmentIdDbQuery>();
        services.AddScoped<IGetAssignsByEquipmentIdDbQuery, GetAssignsByEquipmentIdDbQuery>();
        services.AddScoped<IGetAssignHolderByAssignIdDbQuery, GetAssignHolderByAssignIdDbQuery>();
        services.AddScoped<ISetReturnDateToAssignDbQuery, SetReturnDateToAssignDbQuery>();
        services.AddScoped<IGetEquipmentApproverDbQuery, GetEquipmentApproverDbQuery>();
        services.AddScoped<IUpdateEquipmentDbQuery, UpdateEquipmentDbQuery>();

        services.AddScoped<IEquipmentQueryBuilder, EquipmentQueryBuilder>();
        services.AddScoped<IGetPageableEquipmentsWithFilterOptionsDbQuery, GetPageableEquipmentsWithFilterOptionsDbQuery>();
        services.AddScoped<IGetPossibleEquipmentApproversDbQuery, GetPossibleEquipmentApproversDbQuery>();
        services.AddScoped<IGetPossibleEquipmentActiveHoldersDbQuery, GetPossibleEquipmentActiveHoldersDbQuery>();

        services.AddScoped<IGetEquipmentsDbQuery, GetEquipmentsDbQuery>();

        services.AddScoped<IIsEquipmentUserExistDbQuery, IsEquipmentUserExistDbQuery>();
        services.AddScoped<IIsSerialNumberExistingDbQuery, IsSerialNumberExistingDbQuery>();
        services.AddScoped<IGetEquipmentApproverAndTypeByIdDbQuery, GetEquipmentApproverAndTypeByIdDbQuery>();
        services.AddScoped<ICreateEquipmentDbQuery, CreateEquipmentDbQuery>();
        services.AddScoped<IGetEquipmentUsersLookupDbQuery, GetEquipmentUsersLookupDbQuery>();

        return services;
    }
}
