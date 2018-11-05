package cn.wmyskxz.service;

import cn.wmyskxz.entity.User;

/**
 * @author rookie
 * @date: 2018/11/2 10:38
 */
public interface UserService {

    /**
     * find User By Id
     * @param id
     * @return cn.wmyskxz.entity.User
     **/
    User findUserById(int id);
}
