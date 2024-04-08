﻿using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentActiveHolder;

internal interface IGetPossibleEquipmentActiveHoldersDbQuery : IDbQuery<GetPossibleEquipmentActiveHoldersDbQueryRequest,List<FilterUser>>;
