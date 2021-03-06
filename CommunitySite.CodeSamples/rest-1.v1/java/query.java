﻿public Asset SingleAssetWithAttributes() throws Exception {
        Oid memberId = Oid.fromToken("Member:20", _metaModel);
        Query query = new Query(memberId);
        IAttributeDefinition nameAttribute = _metaModel.getAttributeDefinition("Member.Name");
        IAttributeDefinition emailAttribute = _metaModel.getAttributeDefinition("Member.Email");
        query.getSelection().add(nameAttribute);
        query.getSelection().add(emailAttribute);
        QueryResult result = _services.retrieve(query);
        Asset member = result.getAssets()[0];

        System.out.println(member.getOid().getToken());
        System.out.println(member.getAttribute(nameAttribute).getValue());
        System.out.println(member.getAttribute(emailAttribute).getValue());

        /***** OUTPUT *****
         Member:20
         Administrator
         admin@company.com
         ******************/

        return member;
}