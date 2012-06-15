<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes" />

  <xsl:template match="/" >
    <xsl:apply-templates select="Entity"/>
  </xsl:template>


  <xsl:template match="Entity">
    
      class <xsl:value-of select="LogicalName" />
      {
      
        ///
        /// Constructors
        ///
        public <xsl:value-of select="LogicalName" />()
        {
        }
        <xsl:apply-templates select="Constraints" />
        
        <xsl:apply-templates select="Fields" />

      }
    }
  </xsl:template>
  
  <xsl:template match="Constraints">
    <xsl:apply-templates  select="Constraint[Type = 'PRIMARY KEY' or Type = 'UNIQUE']" />
  </xsl:template >

  <xsl:template match="Constraint">
    
    ///
    ///  Implementation of the tables <xsl:value-of select="Type" /><xsl:value-of select="' '" /><xsl:value-of select="Name" />
    ///
    <xsl:apply-templates  select="Fields" mode="constructor"/>
  </xsl:template >
  
  
  <xsl:template match="Fields" mode="constructor">
      public function __construct(<xsl:value-of select="../../../LogicalName" /> (<xsl:apply-templates  select="Field" mode="constructor"/>)
      {
      }
  </xsl:template >

    <xsl:template match="Field" mode="constructor">
        <xsl:if test="position() != 1" >, </xsl:if>
        $<xsl:value-of select="LogicalName" />
  </xsl:template >
  
  
  <xsl:template match="Fields">
    <xsl:apply-templates  select="Field" />
  </xsl:template >

  <xsl:template match="Field">
      
      /// 
      /// This field represents the data entity field <xsl:value-of select="DBName" />
      ///
      private $<xsl:value-of select="LogicalName" />;
      public function set<xsl:value-of select="LogicalName" />($<xsl:value-of select="LogicalName" />)
      {
        $this-><xsl:value-of select="LogicalName" /> = $<xsl:value-of select="LogicalName" />;
      }
      public function get<xsl:value-of select="LogicalName" />()
      {
        return $this-><xsl:value-of select="LogicalName" />;
      }

  </xsl:template >
  
  
  
  </xsl:stylesheet>