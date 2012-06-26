<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ApiBridge.Web.MVC.ControlPanel.Azure" generation="1" functional="0" release="0" Id="2b2466be-2b15-4339-b4be-2e8e876e90b9" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ApiBridge.Web.MVC.ControlPanel.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ApiBridge.Web.MVC.ControlPanel:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ApiBridge.Web.MVC.ControlPanel.Azure/ApiBridge.Web.MVC.ControlPanel.AzureGroup/LB:ApiBridge.Web.MVC.ControlPanel:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ApiBridge.Web.MVC.ControlPanel:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ApiBridge.Web.MVC.ControlPanel.Azure/ApiBridge.Web.MVC.ControlPanel.AzureGroup/MapApiBridge.Web.MVC.ControlPanel:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ApiBridge.Web.MVC.ControlPanelInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ApiBridge.Web.MVC.ControlPanel.Azure/ApiBridge.Web.MVC.ControlPanel.AzureGroup/MapApiBridge.Web.MVC.ControlPanelInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ApiBridge.Web.MVC.ControlPanel:Endpoint1">
          <toPorts>
            <inPortMoniker name="/ApiBridge.Web.MVC.ControlPanel.Azure/ApiBridge.Web.MVC.ControlPanel.AzureGroup/ApiBridge.Web.MVC.ControlPanel/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapApiBridge.Web.MVC.ControlPanel:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ApiBridge.Web.MVC.ControlPanel.Azure/ApiBridge.Web.MVC.ControlPanel.AzureGroup/ApiBridge.Web.MVC.ControlPanel/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapApiBridge.Web.MVC.ControlPanelInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ApiBridge.Web.MVC.ControlPanel.Azure/ApiBridge.Web.MVC.ControlPanel.AzureGroup/ApiBridge.Web.MVC.ControlPanelInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ApiBridge.Web.MVC.ControlPanel" generation="1" functional="0" release="0" software="C:\GitHub\ApiBridge\ApiBridge.Web.MVC.ControlPanel.Azure\csx\Debug\roles\ApiBridge.Web.MVC.ControlPanel" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ApiBridge.Web.MVC.ControlPanel&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ApiBridge.Web.MVC.ControlPanel&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ApiBridge.Web.MVC.ControlPanel.Azure/ApiBridge.Web.MVC.ControlPanel.AzureGroup/ApiBridge.Web.MVC.ControlPanelInstances" />
            <sCSPolicyFaultDomainMoniker name="/ApiBridge.Web.MVC.ControlPanel.Azure/ApiBridge.Web.MVC.ControlPanel.AzureGroup/ApiBridge.Web.MVC.ControlPanelFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="ApiBridge.Web.MVC.ControlPanelFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ApiBridge.Web.MVC.ControlPanelInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a8845ed2-ad20-4153-9603-727010149dc1" ref="Microsoft.RedDog.Contract\ServiceContract\ApiBridge.Web.MVC.ControlPanel.AzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="dd9d877b-185e-431d-8370-101c4636588c" ref="Microsoft.RedDog.Contract\Interface\ApiBridge.Web.MVC.ControlPanel:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/ApiBridge.Web.MVC.ControlPanel.Azure/ApiBridge.Web.MVC.ControlPanel.AzureGroup/ApiBridge.Web.MVC.ControlPanel:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>