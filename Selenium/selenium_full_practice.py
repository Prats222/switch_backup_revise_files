from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
import time
driver = webdriver.Chrome()

try:
    #driver.get("https://the-internet.herokuapp.com/dropdown")
    #driver.get("https://the-internet.herokuapp.com/javascript_alerts")
    #driver.get("https://the-internet.herokuapp.com/checkboxes")
    driver.get("https://the-internet.herokuapp.com/drag_and_drop")
    driver.fullscreen_window()
    time.sleep(3)
    print(driver.title)
    print(driver.current_url)

    ###alert
    model_page = driver.find_element(By.XPATH, '//*[@id="content"]/div/ul/li[1]/button')
    model_page.click()
    alert = driver.switch_to.alert
    alert.accept()
    #alert.dismiss()
    #alert.send_keys("Prats")

    #------------------------#
    #Dropdown#
    dropdown = Select(driver.find_element(By.ID, "dropdown"))
    time.sleep(2)
    dropdown.select_by_visible_text("Option 2")
    time.sleep(2)

    #------------------------#
    #Checkboxes#
    checkbox = driver.find_element(By.XPATH, "//input[@type='checkbox'][1]")
    if not checkbox.is_selected():
        checkbox.click()

    #------------------------#
    #ActionChains#
    actions = webdriver.ActionChains(driver)
    a = driver.find_element(By.ID,"column-a")
    b = driver.find_element(By.ID,"column-b")
    actions.drag_and_drop(a,b).perform()
    actions.context_click(a).perform()
    actions.double_click(a).perform()
    actions.move_to_element(b).perform()
    driver.execute_script("arguments[0].scrollIntoView();",b)
    #
finally:
    driver.quit()