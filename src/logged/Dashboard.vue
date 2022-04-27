<template>
<div class="text-white">
    <NavbarLogged />
    <div class="p-5">
        <div class="flex justify-center items-center">
            <div class="text-center w-60">
                <p style="color:green; font-size: 20px;"><fa icon="sack-dollar" /> Dólar Comercial </p>
                <input class="text-black rounded-full text-center font-bold mt-2" type="text" :value="'R$ ' + cotacaoDolar" disabled>
            </div>
            <div class="text-center w-60">
                <p style="color:green; font-size: 20px;"><fa icon="money-bill-transfer" /> Conversor</p>
                <input class="text-black rounded-full text-center font-bold mt-2" id="valorDolar" type="text" value="US$ 500">
            </div>
        </div>
        <div class="flex justify-center items-center mt-5">
            <div class="text-center w-60">
                <p style="color:green; font-size: 20px;"><fa icon="brazilian-real-sign" /> Total</p>
                <input class="text-black rounded-full text-center font-bold mt-2" type="text" value="R$ 2400" disabled>
            </div>
        </div>
    </div>
    <div class="flex justify-center items-center">
        <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
            <table class="w-full text-sm text-left text-gray-500 dark:text-gray-400">
                <thead class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                        <th scope="col" class="px-6 py-3">
                            Nome da vaga
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Tipo desenvolvedor
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Modelo de trabalho
                        </th>
                        <th scope="col" class="px-6 py-3">
                            Salário
                        </th>
                        <th scope="col" class="px-6 py-3">
                            <span class="sr-only">Remover</span>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 dark:text-white whitespace-nowrap">
                            Web Developer
                        </th>
                        <td class="px-6 py-4">
                            Full-stack
                        </td>
                        <td class="px-6 py-4">
                            Home office
                        </td>
                        <td class="px-6 py-4">
                            $2999
                        </td>
                        <td class="px-6 py-4 text-right">
                            <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline">Remover</a>
                        </td>
                    </tr>
                    <tr class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 dark:text-white whitespace-nowrap">
                            Arquiteto de cloud
                        </th>
                        <td class="px-6 py-4">
                            Back-end
                        </td>
                        <td class="px-6 py-4">
                            Híbrido
                        </td>
                        <td class="px-6 py-4">
                            $1999
                        </td>
                        <td class="px-6 py-4 text-right">
                            <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline">Remover</a>
                        </td>
                    </tr>
                    <tr class="bg-white dark:bg-gray-800 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 dark:text-white whitespace-nowrap">
                            Desenvolvedor Vue 3 {{ info }}
                        </th>
                        <td class="px-6 py-4">
                            Front-end
                        </td>
                        <td class="px-6 py-4">
                            Presencial
                        </td>
                        <td class="px-6 py-4">
                            $1999
                        </td>
                        <td class="px-6 py-4 text-right">
                            <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline">Remover</a>
                        </td>
                    </tr>
                    <tr class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 dark:text-white whitespace-nowrap">
                            Web Developer
                        </th>
                        <td class="px-6 py-4">
                            Full-stack
                        </td>
                        <td class="px-6 py-4">
                            Home office
                        </td>
                        <td class="px-6 py-4">
                            $2999
                        </td>
                        <td class="px-6 py-4 text-right">
                            <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline">Remover</a>
                        </td>
                    </tr>
                    <tr class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 dark:text-white whitespace-nowrap">
                            Arquiteto de cloud
                        </th>
                        <td class="px-6 py-4">
                            Back-end
                        </td>
                        <td class="px-6 py-4">
                            Híbrido
                        </td>
                        <td class="px-6 py-4">
                            $1999
                        </td>
                        <td class="px-6 py-4 text-right">
                            <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline">Remover</a>
                        </td>
                    </tr>
                    <tr class="bg-white dark:bg-gray-800 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 dark:text-white whitespace-nowrap">
                            Desenvolvedor Vue 3 {{ info }}
                        </th>
                        <td class="px-6 py-4">
                            Front-end
                        </td>
                        <td class="px-6 py-4">
                            Presencial
                        </td>
                        <td class="px-6 py-4">
                            $1999
                        </td>
                        <td class="px-6 py-4 text-right">
                            <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline">Remover</a>
                        </td>
                    </tr>
                    <tr class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 dark:text-white whitespace-nowrap">
                            Web Developer
                        </th>
                        <td class="px-6 py-4">
                            Full-stack
                        </td>
                        <td class="px-6 py-4">
                            Home office
                        </td>
                        <td class="px-6 py-4">
                            $2999
                        </td>
                        <td class="px-6 py-4 text-right">
                            <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline">Remover</a>
                        </td>
                    </tr>
                    <tr class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 dark:text-white whitespace-nowrap">
                            Arquiteto de cloud
                        </th>
                        <td class="px-6 py-4">
                            Back-end
                        </td>
                        <td class="px-6 py-4">
                            Híbrido
                        </td>
                        <td class="px-6 py-4">
                            $1999
                        </td>
                        <td class="px-6 py-4 text-right">
                            <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline">Remover</a>
                        </td>
                    </tr>
                    <tr class="bg-white dark:bg-gray-800 hover:bg-gray-50 dark:hover:bg-gray-600">
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900 dark:text-white whitespace-nowrap">
                            Desenvolvedor Vue 3 {{ info }}
                        </th>
                        <td class="px-6 py-4">
                            Front-end
                        </td>
                        <td class="px-6 py-4">
                            Presencial
                        </td>
                        <td class="px-6 py-4">
                            $1999
                        </td>
                        <td class="px-6 py-4 text-right">
                            <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline">Remover</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <Footer />
</div>
</template>

<script>
import NavbarLogged from '../components/NavbarLogged.vue'
import Footer from '../components/Footer.vue'
import axios from "axios";

export default {
    components: {
        NavbarLogged,
        Footer
    },
    data() {
      return {
          cotacaoDolar: null,
          info: null,
      }  
    },
    mounted () {
        axios.get('https://economia.awesomeapi.com.br/last/USD-BRL').then(response => (this.cotacaoDolar = response.data.USDBRL.bid))
    },
    created () {
        axios.get('https://localhost:5001/v1/usuarios').then(response => (this.info = response.data.nome))
    },  
    calcularDolar(){
        const calcularDolar = document.querySelector('#valorDolar')
        console.log(calcularDolar)
    }
}
</script>

<style lang="postcss" scoped>
body {
   @apply font-semibold;
}
footer{
position: relative;
}
</style>