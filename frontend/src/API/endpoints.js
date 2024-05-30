const logInEP = {
    ep: "/api/login",
    errMsg: "Error login"
};

const organization = {
    ep: "/api/organization/",
    errMsgGetProtocol: "Error getting all Protocols of an organization"
};

const protocol = {
    ep: "/api/protocols",
    errMsgPostProtocol: "Error posting a new protocol"
};


const genericQuery = {
    orgaID: "&organizationId=",
    userID: "&userId=",
    templateID: "?templateId="

}

const genericPath = {
    template: "/templates",
}

export { logInEP, organization, protocol, genericQuery, genericPath };

//postCall(initialPostBody, "/api/protocols?templateId=" + templateID + "&organizationId=" + orgaID + "&userId=" + userID, "Error posting a new protocol", token)