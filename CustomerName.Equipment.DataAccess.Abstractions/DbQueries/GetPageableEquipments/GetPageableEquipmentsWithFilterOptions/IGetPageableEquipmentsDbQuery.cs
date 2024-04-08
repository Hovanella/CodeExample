﻿using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipmentsWithFilterOptions;

internal interface IGetPageableEquipmentsWithFilterOptionsDbQuery : IDbQuery<GetPageableEquipmentsDbQueryRequest, GetPageableEquipmentsWithFilterOptionsDbQueryResponse>;
