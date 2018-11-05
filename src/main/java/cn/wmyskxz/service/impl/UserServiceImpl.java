package cn.wmyskxz.service.impl;

import cn.wmyskxz.entity.User;
import cn.wmyskxz.entity.dao.UserDao;
import cn.wmyskxz.service.UserService;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;

/**
 * UserService Implements
 * @author rookie
 * @date 2018/11/2 10:45
 */
@Service("/userService")
public class UserServiceImpl implements UserService {

    @Resource
    private UserDao userDao;

    @Override
    public User findUserById(int id) {
        return userDao.findUserById(id);
    }
}
