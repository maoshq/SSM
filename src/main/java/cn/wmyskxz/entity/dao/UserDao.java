package cn.wmyskxz.entity.dao;

import cn.wmyskxz.entity.User;
import org.springframework.stereotype.Repository;

/**
 * @author rookie
 * @date: 2018/11/1 19:40
 */
@Repository
public interface UserDao {
    /**
     * find User By Id
     * @Param [id]
     * @return cn.wmyskxz.entity.User
     **/
    User findUserById(int id );
}
