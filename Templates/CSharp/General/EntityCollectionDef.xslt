<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes" />

  <xsl:template match="/" >
    <xsl:apply-templates select="Relation"/>
  </xsl:template>
  
  <xsl:template match="Relation">
    namespace <xsl:value-of select="ChildEntity/Entity/LogicalPackage" />.<xsl:value-of select="ChildEntity/Entity/LogicalModule" />.Collections
    {
      public class <xsl:value-of select="ChildEntity/Entity/LogicalName" />Collection : System.Collections.Generic.List&lt;<xsl:value-of select="ChildEntity/Entity/LogicalName" />&gt;
      {

        ///
        /// Default Constructor
        ///
        public <xsl:value-of select="ChildEntity/Entity/LogicalName" />Collection()
        {
        }


      }
    }
  </xsl:template>
  
  <xsl:template match="Constraints">
    <xsl:apply-templates  select="Constraint[Type = 'PRIMARY KEY']" mode="primarykey" />
    <xsl:apply-templates  select="Constraint[Type = 'UNIQUE']" mode="uniquekey"/>
  </xsl:template >

  <xsl:template match="Constraint" mode="primarykey">
    
    ///
    ///  Implementation of the tables <xsl:value-of select="Type" /><xsl:value-of select="' '" /><xsl:value-of select="Name" />
    ///
    <xsl:apply-templates  select="Fields" mode="constructor"/>
  </xsl:template >
  <xsl:template match="Constraint" mode="uniquekey">

          ///
          ///  Unique key implementations for getting records<xsl:value-of select="Name" />
          ///
    <xsl:apply-templates  select="Fields" mode="uniquekey"/>
  </xsl:template >  
  
  <xsl:template match="Fields" mode="uniquekey">
          public <xsl:value-of select="../../../LogicalName" /> get<xsl:value-of select="../../../LogicalName" /> (<xsl:apply-templates  select="Field" mode="paramList"/>)
          {
            <xsl:value-of select="../../../LogicalName" /> retVal = new <xsl:value-of select="../../../LogicalName" />();

            return retVal;
          }
  </xsl:template >
  
  <xsl:template match="Fields" mode="constructor">
          public <xsl:value-of select="../../../LogicalName" /> (<xsl:apply-templates  select="Field" mode="paramList"/>)
          {
          }
  </xsl:template >
  
    <xsl:template match="Field" mode="paramList">
        <xsl:if test="position() != 1" >, </xsl:if>
        <xsl:value-of select="LogicalType/Name" /><xsl:value-of select="' '" /><xsl:value-of select="LogicalName" />
  </xsl:template >
  
  
  <xsl:template match="Fields">
    <xsl:apply-templates  select="Field" />
  </xsl:template >

  <xsl:template match="Field">
      
          /// 
          /// This field represents the data entity field <xsl:value-of select="DBName" />
          ///
          private <xsl:value-of select="LogicalType/Name" /> _<xsl:value-of select="LogicalName" />;
          public <xsl:value-of select="LogicalType/Name" /><xsl:value-of select="' '" /><xsl:value-of select="LogicalName" /><xsl:if test="/Entity/LogicalName = LogicalName"><xsl:value-of select="LogicalType/Name" /></xsl:if>
          {
            get
            {
              return _<xsl:value-of select="LogicalName" />;
            }
            set
            {
              _<xsl:value-of select="LogicalName" /> = value;
            }
          }
  </xsl:template >
  
  
  
  </xsl:stylesheet>