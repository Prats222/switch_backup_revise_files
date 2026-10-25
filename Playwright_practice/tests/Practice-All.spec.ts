import {test,expect} from '@playwright/test'

test.describe('SauceDemo login Practice @smoke',()=>{
  test.beforeEach(async ({page})=>{
  await page.goto('https://www.saucedemo.com/')
  });

  test('first check',async ({page})=>{
  await expect(page).toHaveTitle(/Swag Labs/);
  await expect(
  page.getByPlaceholder('Username')).toBeVisible()
  });

  test('full site check @full', async ({page})=>{
    var username = await page.getByPlaceholder('Username')
    await username.fill('standard_user')
    var password = await page.getByPlaceholder('Password')
    await password.fill('secret_sauce')

    var submitbtn = page.locator('input.submit-button.btn_action')
    await submitbtn.click()

    // const product = page.locator('.inventry_item').filter({
    //   hasText:'Test.allTheThings() T-Shirt (Red)'
    // });

    // await product.scrollIntoViewIfNeeded()
    // await product.getByRole('button',{name:'Add to cart'}).click()
    const redShirtButton = page.locator('[data-test="add-to-cart-test.allthethings()-t-shirt-(red)"]');
    await redShirtButton.scrollIntoViewIfNeeded();
    await expect (redShirtButton).toBeVisible()
    await redShirtButton.click();

    const cartbutton = page.locator('.shopping_cart_link')
    await cartbutton.click()

    await page.getByRole('button',{name:'Checkout'}).click()

    await page.getByPlaceholder('First Name').fill('Prateek')
    await page.getByPlaceholder('Last Name').fill('Mishra')
    await page.getByPlaceholder('Zip/Postal Code').fill('110092')
    
    await page.getByRole('button',{name:'Continue'}).click()
    await page.getByRole('button',{name:'Finish'}).click()
  
  })

})

