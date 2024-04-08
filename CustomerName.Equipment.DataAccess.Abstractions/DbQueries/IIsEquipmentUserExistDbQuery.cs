﻿using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

internal interface IIsEquipmentUserExistDbQuery : IDbQuery<int, bool>
{
}
