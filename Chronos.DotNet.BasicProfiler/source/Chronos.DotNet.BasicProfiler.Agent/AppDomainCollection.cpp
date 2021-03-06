#include "stdafx.h"
#include "Chronos.DotNet.BasicProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace BasicProfiler
			{
				HRESULT AppDomainCollection::InitializeUnitSpecial(AppDomainInfo* unit)
				{
					unit->InitializeMetadata(_metadataProvider);
					return S_OK;
				}

				const __guid AppDomainCollection::ServiceToken = Converter::ConvertStringToGuid(L"{94B64528-18EC-4103-B0F9-791264886C76}");
			}
		}
	}
}