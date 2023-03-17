using DappervsEF.Models;
using DappervsEF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Http.Results;


namespace DappervsEF.Api
{
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {

        private readonly ProductRepository _productRepository;

        public ProductController()
        {
            _productRepository = new ProductRepository();
        }

        [Route("dapperlist")]
        [HttpGet]
        public Tuple<string, List<Product>> DapperGetAll()
        {
            var products = _productRepository.DapperGetAll();
            string tData = "Total Data : " + products.Count();
            return new Tuple<string,List<Product>>(tData, products);
        }

        [Route("dapperadd")]
        [HttpPost]
        public HttpResponseMessage DapperCreate(Product product)
        {
            var result = _productRepository.DapperInsert(product);
            if (result == null) return Request.CreateResponse(HttpStatusCode.InternalServerError);

            var response = Request.CreateResponse(HttpStatusCode.Created, product);
            response.Headers.Location = new System.Uri(Request.RequestUri, "product/" + product.Id.ToString());

            return response;
        }

        [Route("dapperaddlist")]
        [HttpPost]
        public HttpResponseMessage DapperCreateList(List<Product> products)
        {
            int iSucc=0, iFail=0;
            foreach (var product in products)
            {
               var result = _productRepository.DapperInsert(product);
                if (result == null)
                {
                    iFail++;
                }
                else
                {
                    iSucc++;
                }
            }
            string Message = string.Format("Total Data {0} Success {1} Failed {2}", products.Count, iSucc, iFail);
            var response = Request.CreateResponse(HttpStatusCode.Created, Message);
            return response;
        }

        [Route("dapperaddlistv2")]
        [HttpPost]
        public HttpResponseMessage DapperCreateListV2(List<Product> products)
        {
            var result = _productRepository.DapperInsertV2(products);
            var response = Request.CreateResponse(HttpStatusCode.Created, result);
            return response;
        }

        [Route("dapperupdate")]
        [HttpPost]
        public HttpResponseMessage DapperUpdate(Product product)
        {
            var result = _productRepository.DapperUpdate(product);
            if (result == null) return Request.CreateResponse(HttpStatusCode.NotFound);

            var response = Request.CreateResponse(HttpStatusCode.Accepted, product); 
            if (response == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }




        [Route("eflist")]
        [HttpGet]
        public Tuple<string, List<Product>> EFGetAll()
        {
            var products = _productRepository.EFGetAll();
            string tData = "Total Data : " + products.Count();
            return new Tuple<string, List<Product>>(tData, products);
        }


        [Route("efaddlist")]
        [HttpPost]
        public HttpResponseMessage EFCreateList(List<Product> products)
        {
            int iSucc = 0, iFail = 0;
            foreach (var product in products)
            {
                var result = _productRepository.EFInsert(product);
                if (result == null)
                {
                    iFail++;
                }
                else
                {
                    iSucc++;
                }
            }
            string Message = string.Format("Total Data {0} Success {1} Failed {2}", products.Count, iSucc, iFail);
            var response = Request.CreateResponse(HttpStatusCode.Created, Message);
            return response;
        }


        [Route("efaddlistv2")]
        [HttpPost]
        public HttpResponseMessage EFCreateListV2(List<Product> products)
        {
            var result = _productRepository.EFInsertV2(products);
            var response = Request.CreateResponse(HttpStatusCode.Created, result);
            return response;
        }

    }
}