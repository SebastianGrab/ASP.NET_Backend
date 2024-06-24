import { getCall } from "../API/getCall";
import { postCall } from "../API/postCall";
import { putCall } from "../API/putCall";

export const newUserInputIds = [];

export const getUsers = (token, orgaId) => {
  return (
    getCall(
      "/api/users?pageIndex=1&pageSize=9999999",
      token,
      "Error getting users"
    )
      // return getCall("/api/organization/" + orgaId + " /users?pageIndex=1&pageSize=50", token, "Error getting users")
      .then((response) => {
        console.log("Get Users successful!");
        return response;
      })
  );
};

export const getRoles = (token) => {
  return getCall("/api/roles", token, "Error getting roles").then(
    (response) => {
      console.log("Get Roles successful!");
      return response;
    }
  );
};

export const findIdByName = (name, data) => {
  for (let i = 0; i < data.length; i++) {
    if (data[i].name === name) {
      return data[i].id;
    }
  }
  return null;
};
export const roleInformationAsArrays = (roleData) => {
  let result = {
    roleNames: [],
    roleIds: [],
  };

  for (let i = 0; i < roleData.length; i++) {
    result.roleNames[i] = roleData[i].name;
    result.roleIds[i] = roleData[i].id;
  }

  return result;
};

export const postUser = async (token, data, roleId) => {
  const stringData = buildUserData(data);

  try {
    const response = await postCall(
      stringData,
      "/api/users",
      "Error posting a new user",
      token
    );
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
};

export const updateUser = async (token, data, userId) => {
  data.id = userId;
  delete data.role;
  delete data.organization;
  data.username = data.firstName + " " + data.lastName;
  const stringData = JSON.stringify(data);

  try {
    const response = await putCall(
      stringData,
      "/api/user/" + userId,
      "Error updating a User",
      token
    );
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
};

export const buildUserData = (data) => {
  const userInformation = data;
  delete userInformation.role;
  delete userInformation.orgaName;
  const stringData = JSON.stringify(userInformation);
  return stringData;
};

export const postRoleId = async (token, roleId, orgaId, userId) => {
  try {
    const response = await postCall(
      "",
      "/api/users",
      "Error posting a new user",
      token
    );
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
};

export const findRoleIdByName = (name, roleArray) => {
  let id = null;

  roleArray.forEach((element) => {
    if (element.name === name) {
      id = element.id;
    }
  });
  return id;
};

export const buildTableData = (userData, token) => {
  userData.forEach((a) => {
    const fetch = async (a) => {
      let role = await getCall(
        "/api/user/" + a.id + "/roles",
        token,
        "Fehler beim Rollen holen"
      );
      let organization = await getCall(
        "/api/user/" + a.id + "/organizations",
        token,
        "Fehler beim Orga holen"
      );
      if (role.length > 0) {
        a.role = role[0].name;
      }
      if (organization.length > 0) {
        a.organization = organization[0].name;
      }
    };
    fetch(a);
  });

  return userData;
};
