<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  
  <?xml version="1.0"?>
<!DOCTYPE hibernate-mapping PUBLIC
   "-//Hibernate/Hibernate Mapping DTD//EN"
   "http://hibernate.sourceforge.net/hibernate-mapping-2.0.dtd">
<hibernate-mapping>
   <class
      name="hello.Message"
      table="MESSAGES">
      <id
         name="id"
         column="MESSAGE_ID">
         <generator class="increment"/>
      </id>
      <property
         name="text"
         column="MESSAGE_TEXT"/>
      <many-to-one
         name="nextMessage"
         cascade="all"
         column="NEXT_MESSAGE_ID"/>
   </class>
</hibernate-mapping>
  

</xsl:stylesheet>