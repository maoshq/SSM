package cn.wmyskxz.controller;

import cn.wmyskxz.entity.User;
import cn.wmyskxz.service.UserService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;

import javax.annotation.Resource;


/**
 * @author rookie
 * @date 2018/11/2 10:57
 */
@Controller
@RequestMapping("")
public class UserController {

    @Resource
    private UserService userService;

    @RequestMapping("/findUser")
    public String findUser(Model model) {
        int id = 1;
        User user = this.userService.findUserById(id);
        model.addAttribute("user", user);
        return "index";

    }
}