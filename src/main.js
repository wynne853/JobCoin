import { createApp } from 'vue'
import { createWebHistory, createRouter,  } from "vue-router";
import App from './App.vue'
import VueSweetalert2 from 'vue-sweetalert2';
import money from 'vuejs-money'
import Notification from 'notiwind'

//views
import Home from './Home'
import Login from './Login'
import Register from './Register'
import Dashboard from './logged/Dashboard'

//css
import './assets/tailwind.css'
import 'sweetalert2/dist/sweetalert2.min.css';

//FontAwesome

//middlewares
// import Guard from './middleware/auth';

import store from './store'




const routes = [
    {
      name: "Home",
      path: "/",
      component: Home,
    },
    {
      name: "Login",
      path: "/entrar",
      component: Login,
    },
    {
      name: "Registrar",
      path: "/registro",
      component: Register,
    },
    {
      name: "Dashboard",
      path: "/dashboard",
      component: Dashboard,
    }
  ];

const router = createRouter({
  history: createWebHistory(),
  routes,
});


const app = createApp(App).use(store)
app.use(router);
app.mount('#app');
app.use(VueSweetalert2);
app.use(money);
app.use(Notification);