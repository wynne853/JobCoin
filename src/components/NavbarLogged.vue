<template>
  <div class="bg-gray-800 rounded-b-lg">
    <div
      class="px-4 py-5 mx-auto sm:max-w-xl md:max-w-full lg:max-w-screen-xl md:px-24 lg:px-8"
    >
      <div class="relative flex grid items-center grid-cols-2 lg:grid-cols-3">
        <ul class="flex items-center hidden space-x-8 lg:flex">
          <li>
            <router-link to="/dashboard" :class="{'font-medium tracking-wide bg-gray-700 py-2 px-3 rounded-md text-gray-100 transition-colors duration-200 hover:bg-gray-500 transform transition duration-1000 hover:scale-90 cursor-pointer' : $route.name === 'index', 
            'font-medium tracking-wide text-gray-100 transition-colors duration-200 hover:text-teal-accent-400 transform transition duration-1000 hover:scale-90 cursor-pointer' : $route.name !== 'index' } ">
              Inicio
            </router-link>
          </li>
          <li>
            <router-link to="/vagas" :class="{'font-medium tracking-wide bg-gray-700 py-2 px-3 rounded-md text-gray-100 transition-colors duration-200 hover:bg-gray-500 transform transition duration-1000 hover:scale-90 cursor-pointer' : $route.name === 'categories', 
            'font-medium tracking-wide text-gray-100 transition-colors duration-200 hover:text-teal-accent-400 transform transition duration-1000 hover:scale-90 cursor-pointer' : $route.name !== 'categories' } ">
              Vagas
            </router-link>
          </li>
          <li>
            <router-link to="/cadastrar-vagas" :class="{'font-medium tracking-wide bg-gray-700 py-2 px-3 rounded-md text-gray-100 transition-colors duration-200 hover:bg-gray-500 transform transition duration-1000 hover:scale-90 cursor-pointer' : $route.name === 'categories', 
            'font-medium tracking-wide text-gray-100 transition-colors duration-200 hover:text-teal-accent-400 transform transition duration-1000 hover:scale-90 cursor-pointer' : $route.name !== 'categories' } ">
              Cadastrar vaga
            </router-link>
          </li>
        </ul>
        <router-link to="/" class="inline-flex items-center lg:mx-auto">
           <svg class="w-6 h-6 text-white 3xl" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 9V7a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2m2 4h10a2 2 0 002-2v-6a2 2 0 00-2-2H9a2 2 0 00-2 2v6a2 2 0 002 2zm7-5a2 2 0 11-4 0 2 2 0 014 0z"></path></svg>
          <span
            class="ml-2 text-xl font-bold tracking-wide text-gray-100"
            >JobCoin</span
          >
        </router-link>
        
        <ul class="flex items-center hidden ml-auto space-x-8 lg:flex">
          <li>
            <router-link to="/">
            <a
              class="inline-flex items-center justify-center h-12 px-6 font-medium tracking-wide text-white transition duration-200 rounded shadow-md bg-gray-700 hover:bg-deep-purple-accent-700 focus:shadow-outline focus:outline-none transform transition duration-1000 hover:scale-90 cursor-pointer"
              aria-label="Sair"
              title="Sair"
            >
              Sair
            </a>
            </router-link>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script>
export default {

}
</script>

